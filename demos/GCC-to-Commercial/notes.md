## For Event Broker
Example blob storage webhook validation "handshake":
```
[
  {
    "id": "59b21e8f-42ae-4902-9066-d5185a1e747b",
    "topic": "/subscriptions/b8093588-4b1f-49d6-8e3d-cebb694d529e/resourceGroups/TIMH-CRUX/providers/Microsoft.Storage/storageAccounts/timhcrux",
    "subject": "",
    "data": {
      "validationCode": "2C959237-B987-49C1-B4C2-BB02D7318EEC",
      "validationUrl": "https://rp-eastus.eventgrid.azure.net:553/eventsubscriptions/test/validate?id=2C959237-B987-49C1-B4C2-BB02D7318EEC&t=2023-04-21T15:10:27.3484420Z&apiVersion=2021-10-15-preview&token=fxiFFLdyILur53xx%2fB1wfSC1Ak%2fqzEYeO04z%2fGz9yfI%3d"
    },
    "eventType": "Microsoft.EventGrid.SubscriptionValidationEvent",
    "eventTime": "2023-04-21T15:10:27.348442Z",
    "metadataVersion": "1",
    "dataVersion": "2"
  }
]
```

Example blob storage event notification to that webhoook, after handshake, alerting that a blob has been created:
```
[
  {
    "topic": "/subscriptions/b8093588-4b1f-49d6-8e3d-cebb694d529e/resourceGroups/TIMH-CRUX/providers/Microsoft.Storage/storageAccounts/timhcrux",
    "subject": "/blobServices/default/containers/test/blobs/20230421_085348.jpg",
    "eventType": "Microsoft.Storage.BlobCreated",
    "id": "45cc4533-001e-0061-5163-74b42506e59b",
    "data": {
      "api": "PutBlob",
      "clientRequestId": "a955d536-4035-4bbb-8933-978b4997d01a",
      "requestId": "45cc4533-001e-0061-5163-74b425000000",
      "eTag": "0x8DB427AB43FE83C",
      "contentType": "image/jpeg",
      "contentLength": 35496,
      "blobType": "BlockBlob",
      "url": "https://timhcrux.blob.core.windows.net/test/20230421_085348.jpg",
      "sequencer": "00000000000000000000000000008E400000000000251b5c",
      "storageDiagnostics": {
        "batchId": "1bf1f95f-e006-0046-0063-74a3e1000000"
      }
    },
    "dataVersion": "",
    "metadataVersion": "1",
    "eventTime": "2023-04-21T15:11:38.0597577Z"
  }
]
```
