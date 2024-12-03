# E.V.V.I.E.: Enterprise Visual Vehicle Inspection Engine
E.V.V.I.E. is a proof-of-concept system that uses advanced artificial intelligence models to automatically evaluate and document vehicle damage for an organization's fleet.

**E.V.V.I.E. is still in development - more coming soon, stay tuned!**

## E.V.V.I.E. Source Code
The source code of E.V.V.I.E. is provided in the `src` folder as follows:
- [core](./src/core/) - this contains a C# console application that is used essentially as a library of functions and capabilities that E.V.V.I.E. relies on for communicating with the Azure OpenAI service. This allows E.V.V.I.E. to reach out to the Azure OpenAI service to do things like identify vehicles via their license plate and assess damage to vehicles.
- [api](./src/api/) - this contains the code to a **v4**, **.NET 8.0-based** Azure Function, written in C#, that exposes two endpoints that the E.V.V.I.E. interface, built in Power Apps, can call to. Those endpoints are `/plate`, reading a license plate number from a single provided image in *base64* format, and `/inspect`, assessing the damage to a vehicle based on one or multiple provided images of the damage to the car in *base64* format.