# Project Brief

ReplicaStock is a C# .NET integration service designed to replicate and synchronize inventory stock levels and supplier information to a remote system (Napse Global Bridge) via SOAP web services.

## Core Requirements & Goals
- Extract, format, and send data (Stock and Supplier) to a central system using SOAP XML envelopes.
- Abstract the creation of XML requests for different entities (`Stock`, `Supplier`).
- Execute SOAP requests to `BridgeCoreSOAPServiceClient`.
- Provide logging and configuration utilities to manage the integration.
