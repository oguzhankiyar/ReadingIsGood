# Reading Is Good
This is a sample project including identity, product and order modules for e-commerce

This project is designed with modular monolith approach, it makes microservice migration easy

There are two steps to achieve this:
- Implement message bus for a message broker
- Separate all modules to own host project

## Structure
### Host
The web project to expose identity, product and order endpoints

### Identity
The library projects for identity module
- API for auth and users controllers
- Business for auth and users handlers, mappings and validations
- Contracts for models, requests and responses for identity module
- Persistence for entity, configs and context for identity module

#### Product
The library projects for product module
- API for products controller
- Business for products handlers, consumers, mappings and validations
- Contracts for models, requests and responses for product module
- Persistence for entity, configs and context for product module

### Order
The library projects for order module
- API for identity orders controller
- Business for orders handlers, mappings and validations
- Contracts for models, requests and responses for order module
- Persistence for entity, configs and context for order module

### Shared
The library projects for common purpose of all modules
- Core for models, events, extensions, requests, responses
- MessageBus for message bus abstraction and implementation for communication between modules
- Persistence for base domains, entities, configs and base context for changeset process

## Run
- Clone the repository
```console
git clone https://github.com/oguzhankiyar/ReadingIsGood
```

- Go to solution directory
```console
cd ReadingIsGood
```

- Build a docker container
```console
docker-compose up -d
```

- Open port 3333
```console
start http://127.0.0.1:3333
```

### Happy Coding!