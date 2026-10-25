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

    params {
        param("release.flag", "false")
        param("app.version", "0.0.0")
    }

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        dotnetBuild {
            name = "Build"
            id = "Build"
            projects = "GestionPedidos.slnx"
            framework = "net10.0"
            configuration = "Release"
            args = "-maxcpucount:1"
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
