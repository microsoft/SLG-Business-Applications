# EVVIE API Endpoints
This page documents the API endpoints and their input/output schemas for [EVVIE's ASP.NET API](../src/api/).

## Check that the Azure Function is on: `/version`

```
GET /version
```

Example response:

```
200 OK
0.2.0
```

## Read License Plate: `/plate`
Use the `/plate` endpoint's ability to read a license plate. Provide the image of the license plate as a base64 string.

```
POST /plate
Content-Type: application/json

{
    "image": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABA..."
}
```

Example response:
```
200 OK
Content-Type: application/json

{
    "plate": "H452J9"
}
```

## Read Odometer: `/odometer`
Use the `/odometer` endpoint to read the odometer reading from a picture. Provide the image of the odometer as a base64 string:

Example request:

```
POST /odometer
Content-Type: application/json

{
    "image": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABA..."
}
```

Example response:

```
200 OK
Content-Type: application/json

{
    "reading": 123456
}
```

## Validate Image Quality: `/validate`
Before EVVIE proceeds with the inspection, it first performs a validation of the provided images' quality. It verifies that the images are of a high enough quality to actually be used in an inspection. It validates things like that the image is not blurry, is not in poor lighting, is not obstructed, etc.

Example request:

```
POST https://timh-evvie.azurewebsites.net/inspect
Content-Type: application/json

{
    "images":
    [
        "data:image/jpeg;base64,/9j/4AAQSkZJRgA...",
        "data:image/jpeg;base64,/9j/45AsJkNJTuv..."
    ]
}
```

Example response:

```
200 OK
Content-Type: application/json

{
  "issues": [
    {
      "title": "Image is Blurry",
      "description": "The image is very out of focus, making it difficult to spot any damage. Please ensure the camera is steady and properly focused when taking the photo."
    },
    {
      "title": "Seek Better Lighting",
      "description": "The lighting in the image is dim and makes it difficult to accurately observe the condition of the vehicle. Please try to take the picture in a brighter environment or use additional lighting."
    },
    {
      "title": "Stand Closer",
      "description": "The photo appears to be taken from too far away from the vehicle, and it is hard to clearly see any specific details. Please stand closer, within 15 feet, and ensure the area with possible damage is clearly visible."
    }
  ]
}
```

As you can see above, the quality issues with the supplied image(s) are provided as an array of strings under the `issues` property. If there are no issues with the captured photos, the `issues` array will simply be empty (an empty array)!

## Inspect Vehicle Images for Damage: `/inspect`
Use the `/inspect` endpoint to inspect pictures of a vehicle for damage. Provide one or multiple images of the vehicle as the `images` property, encoded in base64 (an array of strings).

Example request:

```
POST https://timh-evvie.azurewebsites.net/inspect
Content-Type: application/json

{
    "images":
    [
        "data:image/jpeg;base64,/9j/4AAQSkZJRgA...",
        "data:image/jpeg;base64,/9j/45AsJkNJTuv..."
    ]
}
```

Example response:

```
200 OK
Content-Type: application/json

{
    "area": 5,
    "description": "Significant dents and scratches with a partially detached bumper on the front right side.",
    "severityLevel": 3
}
```