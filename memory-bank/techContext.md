# Tech Context

## Technologies Used
- C# / .NET (Console Application)
- System.ServiceModel (WCF) for defining and communicating via SOAP
- Newtonsoft.Json (Version 12.0.2)

## Development Setup
- Visual Studio environment.
- NuGet packages managed via `packages.config`.
- Application configuration driven by `App.config` and a custom JSON based configuration via `Resources/AppConfig.json`.

## Technical Constraints
- Integration specifically targets Napse Bridge API, requiring precise XML structure.
- The WCF endpoint is HTTPS (`https://bmc.dev.napse.global:45503/bridge/services/bridgeCoreSOAP`).

## Tool Usage Patterns
- `XMLCreator` and `XmlHelper` are used for generating valid XML representations of domain entities.
- `SOAPSender` manages the actual outbound HTTP request for services that don't rely fully on the WCF client wrapper.
