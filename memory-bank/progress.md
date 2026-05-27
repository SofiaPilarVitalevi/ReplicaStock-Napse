# Progress

## What works
- Basic application skeleton is in place with separate logic layers (`Entidades`, `Negocio`, `Utils`).
- External WCF service reference (`BridgeCoreSOAPServiceClient`) is generated.
- Hardcoded XML generation and sending for `Stock` and `Supplier` is functionally drafted.
- Configuration and logging components are established.

## What's left to build
- Make the `Supplier` logic dynamic instead of using hardcoded XML envelopes.
- Re-activate and fully test `Stock` updating process.
- Implement error handling and possible retry mechanisms for SOAP delivery.
- Handle data source reading (e.g. fetching actual Stock or Supplier data from a local database).

## Current status
- The application currently operates in a testing mode where `Supplier` updates are mocked and executed manually through the `Program` entry point.
