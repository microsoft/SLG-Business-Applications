# EVVIE Tests
You can run the following tests below to test the viability of EVVIE's Azure Function deployment:

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
    "issues":
    [
        "Lighting is poor: move to a more well lit area.",
        "The image is blurry: make sure your hand is still when taking the photo"
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