# System Patterns

## System Architecture
The application follows a traditional multi-layered architectural pattern:
- **Presentation / Entry Point (`Program.cs`)**: A console application that initiates the sync jobs.
- **Business Logic Layer (`Negocio`)**: Contains controllers like `Stock` and `Supplier` that manage the creation and dispatching of data.
- **Data Entities (`Entidades`)**: POCO classes (`Stock`, `Item`, `Supplier`, `NapseEntity`) used to model the business domain.
- **Utilities / Cross-Cutting Concerns (`Utils`)**: Provides configuration management (`Config`), logging (`Logger`), and communication/serialization (`SOAPSender`, `XMLCreator`, `XmlHelper`).

## Key Technical Decisions
- **SOAP Integration**: The communication protocol for the Napse Bridge is SOAP. The application manually constructs some SOAP envelopes or utilizes native WCF service clients (`Connected Services/ServiceReference1`).
- **Data Formatting**: The payload needs to be structured in specific XML schemas (`<bridgeCoreRequest>`), with nested lists representing the data.

## Component Relationships
`Program` -> Calls `Negocio` classes -> Converts `Entidades` into XML using `Utils` -> Sends via HTTP using `SOAPSender`.
