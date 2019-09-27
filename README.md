
# PaymentGateway

An in-process implementation of a payment gateway using Event Sourcing, CQRS, .NET Core, Streamstone, MediatR and Azure Table Storage.

## Requirements
To play with the API, you either need an Azure Table Storage account or to install Microsoft Azure Storage Emulator. This application has been tested with Microsoft Azure Storage Emulator v5.9 and Visual Studio 2019 Professional.

If you wish to move from the emulator to a real Azure Table Storage, just update the connection string in the `appsettings.json` file from the `PaymentGateway.Api` project:
```json
"Storage": {
    "ConnectionString": "UseDevelopmentStorage=true"
}
```

## Setup


Run the API project `PaymentGateway.Api` and go to the swagger URL if it doesn't open automatically: [http://localhost:55666/swagger/index.html](http://localhost:55666/swagger/index.html)

Make a POST request to the infrastructure route : 
![](https://i.imgur.com/UD9xTk6.png)

That will set up the required infrastructure to execute integration tests and play with the Api.

## Event store
The event store is managed by the [Streamstone]([https://github.com/yevhen/Streamstone](https://github.com/yevhen/Streamstone)) library. You can browse the event store with `Microsoft Azure Storage Explorer`, within the table named `eventstore` :

![](https://i.imgur.com/xA7lKYn.png)

## Projections

Projections are managed through event handler using the [MediatR]([https://github.com/jbogard/MediatR](https://github.com/jbogard/MediatR)) library.

You can browse the payment projection view with `Microsoft Azure Storage Explorer`, within the table named `payments` :


![](https://i.imgur.com/8zeXF4x.png)

## FAQ

### Why ValueObjects can't have a public constructor?
I've chosen to embed value object within events. Events are immutable and **must** always be deserializable to load an aggregate root. On the opposite, business rules regarding validation of value objects may change over time. Therefore, if we put validation in a public constructor of a value object, we could end up not being able to deserialize older events due to validation violation in embedded value objects.

They are many possibilities to overcome this problem:

 - Don't use value object in events. I'm not a huge fan of this, because that means that the aggregate root must know *how* to build a value object. If the later one has a lot of parameters, that becomes easily cumbersome.
 - Use a binary serializer instead of JSON. Using a binary serializer like Protobuf or Avro would deserialize an event without calling any constructor. And it's faster. But I didn't want to add the complexity of managing proto files in this demo project.
 - Move the validation outside the constructor, and use a static factory. That's what I've decided to do in this project.


 
