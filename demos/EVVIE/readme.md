# E.V.V.I.E.: Enterprise Visual Vehicle Inspection Engine
E.V.V.I.E. is a proof-of-concept system that uses advanced artificial intelligence models to automatically evaluate and document vehicle damage for an organization's fleet.

**E.V.V.I.E. is still in development - more coming soon, stay tuned!**

## E.V.V.I.E. Robot Graphics
All GIF graphics of EVVIE are in the [graphics folder](./graphics/).

## Potential Car Sketches to Use for Mapping Damage
- https://i.imgur.com/WRsVzjt.jpeg
    - Front: https://imgur.com/wF1aA3f
    - Back: https://imgur.com/ntBncTK
    - Left: https://imgur.com/ae8ZHJq
    - Right: https://imgur.com/8GFKQ3j
- https://i.imgur.com/ycmSEU7.jpeg
- https://i.imgur.com/svcpOyq.jpeg

## We were not able to load some functions in the list due to errors
Encountered this odd error in Azure Functions on November 21, 2024. It has to do with the Azure Function not having connection string access to the Azure Storage Account is is set up to use (in the App Settings).

Due to policy, the "Allow storage account key access" toggle had flipped to "Disabled". To fix this error, flip this back to "Enabled", wait 5 minutes, and then restart the Azure Function.