import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.PullRequests
import jetbrains.buildServer.configs.kotlin.buildFeatures.commitStatusPublisher
import jetbrains.buildServer.configs.kotlin.buildFeatures.pullRequests
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetBuild
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2026.2"

project {
    description = "Create a dotnet portfolio to enhance the skills"

    buildType(SmokeTest)
    buildType(Build)
}

object Build : BuildType({
    name = "Build"
    description = "-maxcpucount:1 avoid two MSBuild nodes simultaneous builds targeting the same project and deps.json"

    params {
        param("release.flag", "false")
        param("app.version", "0.0.0")
    }

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        script {
            name = "Versioning"
            id = "Versioning"
            scriptContent = """
                #!/bin/bash
                set -eu
                cd %system.teamcity.build.checkoutDir%
                
                # Brings default tags
                git fetch --tags origin
                
                MAJOR=${'$'}(sed -n 's:.*<VersionMajor>\([0-9]*\)</VersionMajor>.*:\1:p' Directory.Build.props)
                MINOR=${'$'}(sed -n 's:.*<VersionMinor>\([0-9]*\)</VersionMinor>.*:\1:p' Directory.Build.props)
                
                # Release only in default branch and with logic or version change sice last tag
                RELEASE=false
                if [ "%teamcity.build.branch.is_default%" = "true" ]; then
                   LAST_TAG=${'$'}(git describe --tags --abbrev=0 --match "[0-9]*.[0-9]*.[0-9]*" 2>/dev/null || true)
                   if [ -z "${'$'}LAST_TAG" ]; then
                      RELEASE=true
                   elif git diff --name-only "${'$'}LAST_TAG" HEAD | grep -qE '^(src/GestionPedidos\.(Domain|Application|Api|Infrastructure)/Directory\.Build\.props)'; then
                         RELEASE=true
                   fi
                fi
                
                # Hotfix: Last tag of MAJOR.MINOR.x + 1: Otherelse: 1
                LAST_HOTFIX=${'$'}(git tag --list "${'$'}{MAJOR}.${'$'}{MINOR}.*" | sort -V | tail -n1)
                
                if [ "${'$'}RELEASE" = true ]; then
                   HOTFIX=1
                   if [ -n "${'$'}LAST_HOTFIX" ]; then
                      HOTFIX=${'$'}(( ${'$'}{LAST_HOTFIX##*.} + 1 ));
                   fi
                   VERSION="${'$'}{MAJOR}.${'$'}{MINOR}.${'$'}{HOTFIX}"
                   echo "##teamcity[buildNumber '${'$'}{VERSION}']"
                else
                   # Dev: last publish release. 1 if none.
                   BASE_HOTFIX=1
                   if [ -n "${'$'}LAST_HOTFIX" ]; then
                       BASE_HOTFIX="${'$'}{LAST_HOTFIX##*.}";
                   fi
                   VERSION="${'$'}{MAJOR}.${'$'}{MINOR}.${'$'}{BASE_HOTFIX}"
                   echo "##teamcity[buildNumber '${'$'}{VERSION}-dev.%build.counter%']"
                fi
                echo "##teamcity[setParameter name='app.version' value='${'$'}{VERSION}']"
                echo "##teamcity[setParameter name='release.flag' value='${'$'}{RELEASE}']"
            """.trimIndent()
        }
        dotnetBuild {
            name = "Build"
            id = "Build"
            projects = "GestionPedidos.slnx"
            framework = "net10.0"
            configuration = "Release"
            args = "-maxcpucount:1"
        }
        script {
            name = "temporal"
            id = "temporal"
            enabled = false
            scriptContent = """
                cd %teamcity.build.checkoutDir%
                echo "-- local tags --"
                git tag --list
                echo "-- remoto tags --"
                git ls-remote --tags origin || echo "fail ls-remote"
                echo "--- fecth tags --"
                git fetch --tags origin || echo "fail fetch"
                echo "-- tags after fetch --"
                git tag --list
            """.trimIndent()
        }
    }

    triggers {
        vcs {
            branchFilter = """
                +:<default>
                +pr: target=main
                -pr: sourceRepo=fork
            """.trimIndent()
        }
    }

    features {
        pullRequests {
            vcsRootExtId = "${DslContext.settingsRoot.id}"
            provider = github {
                authType = token {
                    token = "credentialsJSON:a0b7f4ce-df22-4bc9-8370-2fe9fd441bbd"
                }
                filterTargetBranch = "+:refs/heads/main"
                filterAuthorRole = PullRequests.GitHubRoleFilter.MEMBER
                discoveryMode = PullRequests.branchesDiscoveryMode.prRefs
            }
        }
        commitStatusPublisher {
            vcsRootExtId = "${DslContext.settingsRoot.id}"
            publisher = github {
                statusCheckName = "Build gestión pedidos"
                githubUrl = "https://api.github.com"
                authType = personalToken {
                    token = "credentialsJSON:a0b7f4ce-df22-4bc9-8370-2fe9fd441bbd"
                }
            }
        }
    }
})

object SmokeTest : BuildType({
    name = "Smoke Test"
    description = "Create a build without repository"

    steps {
        script {
            name = "Test"
            id = "Test"
            scriptContent = """
                echo "Agents OK on ${'$'}(uname -a)"
                echo "Dotnet version: ${'$'}(dotnet --version)"
            """.trimIndent()
        }
    }
})
