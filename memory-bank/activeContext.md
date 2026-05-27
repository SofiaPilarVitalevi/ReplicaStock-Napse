# Active Context

## Current Work Focus
- The system is currently testing or developing the `Supplier` synchronization logic.
- In `Program.cs`, `Negocio.Supplier().CreateUpdateSupplierDestino()` is active.
- The `Supplier` logic uses a hardcoded XML SOAP envelope for testing purposes rather than fully mapping from entities at the moment.

## Recent Changes
- Initial implementation of the `memory-bank` documentation.
- The `Stock.CreateUpdatereplicaStockDestino()` is currently commented out in the main execution path.

## Next Steps
- Verify if the `Supplier` SOAP request needs to be dynamic instead of hardcoded.
- Potentially reactivate and test the `Stock` synchronization logic.
- Ensure the connection details (WCF client endpoints and `SOAPSender` logic) are properly configured for PROD vs TEST environments (as indicated by the `PUB/PROD` and `PUB/TEST` folders).

## Active Decisions and Considerations
- Some XML creation relies on string concatenation (`StringBuilder`), while other parts may use serialization (`XmlHelper`). A unified approach to XML creation might be needed depending on the complexity of the Napse API requirements.
