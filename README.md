# netcore-rmq

# RabbitMQ

## Baixar imagem do RabbitMQ
docker pull rabbitmq:management

## Executar o container do RabbitMQ
docker run --rm -d --hostname rabbitmq --name rabbit-mq -p 15672:15672 -p 5672:5672 rabbitmq:3-management

# Kafka
docker pull bitnami/kafka

## Baixar imagem do Kafka
docker run --rm -d --hostname kafka --name kafka

