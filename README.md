# Azure Functions Node.js Worker

|Branch|Status|[Runtime Version](https://docs.microsoft.com/azure/azure-functions/functions-versions)|Support level|Node.js Versions|
|---|---|---|---|---|
|v3.x (default)|[![Build Status](https://img.shields.io/azure-devops/build/azfunc/Azure%2520Functions/10/v3.x)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=10&branchName=v3.x) [![Test Status](https://img.shields.io/azure-devops/tests/azfunc/Azure%2520Functions/99/v3.x?compact_message)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=99&branchName=v3.x)|4|GA (Recommended)|16 (Preview), 14|
|v2.x|[![Build Status](https://img.shields.io/azure-devops/build/azfunc/Azure%2520Functions/10/v2.x)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=10&branchName=v2.x) [![Test Status](https://img.shields.io/azure-devops/tests/azfunc/Azure%2520Functions/99/v2.x?compact_message)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=99&branchName=v2.x)|3|GA|14, 12, 10|
|v1.x|[![Build Status](https://img.shields.io/azure-devops/build/azfunc/Azure%2520Functions/10/v1.x)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=10&branchName=v1.x) [![Test Status](https://img.shields.io/azure-devops/tests/azfunc/Azure%2520Functions/10/v1.x?compact_message)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=10&branchName=v1.x)|2|GA (Maintenance mode)|10, 8|

> NOTE: The branch corresponds to the _worker_ version, which is intentionally decoupled from the _runtime_ version.

## Getting Started

- [Create your first Node.js function](https://docs.microsoft.com/azure/azure-functions/create-first-function-vs-code-node)
- [Node.js developer guide](https://docs.microsoft.com/azure/azure-functions/functions-reference-node)
- [Language Extensibility Wiki](https://github.com/Azure/azure-webjobs-sdk-script/wiki/Language-Extensibility)

## Contributing

- Clone the repository locally and open in VS Code
- Run "Extensions: Show Recommended Extensions" from the [command palette](https://code.visualstudio.com/docs/getstarted/userinterface#_command-palette) and install all extensions listed under "Workspace Recommendations"
- Run `npm install` and `npm run build`
- Create or open a local function app to test with
- In the local function app, add the following settings to your "local.settings.json" file or configure them directly as environment variables
  - `languageWorkers__node__workerDirectory`: `<path to the root of this repository>`
  - `languageWorkers__node__arguments`: `--inspect`
    > 💡 Tip #1: Set `logging__logLevel__Worker` to `debug` if you want to view worker-specific logs in the output of `func start`

    > 💡 Tip #2: If you need to debug worker initialization, use `--inspect-brk` instead of `--inspect`. Just keep in mind you need to attach the debugger within 30 seconds or the host process will timeout.
- Start the local function app (i.e. run `func start` or press <kbd>F5</kbd>)
- Back in the worker repository, press <kbd>F5</kbd> and select the process for your running function app
- Before you submit a PR, run `npm run lint` and `npm test` and fix any issues. If you want to debug the tests, switch your [launch profile](https://code.visualstudio.com/docs/editor/debugging) in VS Code to "Launch Unit Tests" and press <kbd>F5</kbd>.


### Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

### Contributing to TypeScript type definitions

The type definitions supplied by the `@azure/functions` [npm package](https://www.npmjs.com/package/@azure/functions) are located in the `types` folder. Any changes should be applied directly to `./types/index.d.ts`. Please make sure to update the tests in `./types/index.test.ts` as well.