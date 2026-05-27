# Product Context

## Why this project exists
Retail businesses and distributed store environments often need to keep their point of sale or central ERP systems updated with current inventory levels and supplier data. ReplicaStock bridges a local system's data and updates the "Napse Bridge" platform using SOAP APIs.

## Problems it solves
- Ensures inventory tracking (Stock) is accurately reflected in external systems.
- Synchronizes supplier registry data.
- Handles the complex XML structuring and SOAP enveloping needed by the Napse Bridge API.

## How it should work
- It executes jobs/tasks (either manually or via a scheduler depending on deployment) that instantiate business logic classes (like `Negocio.Stock` and `Negocio.Supplier`).
- These classes map local entities (`Entidades`) to XML format.
- The XML is then wrapped in a SOAP envelope and sent via HTTP/HTTPS.
