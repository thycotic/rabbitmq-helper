[title]: # (RabbitMq Load Balancing)
[tags]: # (rabbitmq,load balancing)
[priority]: # (1500)

# Load-Balancing

The Helper does not provide any assistance with TCP load balancing RabbitMQ. That is also out of the scope for RabbitMQ itself. You are can use any load-balancer you have access to.

> Possible load-balancers include but not limited to: F5, NetScaler, HAProxy.

## HAProxy on Ubuntu example for POC

```bash				
sudo apt-get install openssh-server
sudo apt-get install haproxy
sudo nano /etc/haproxy/haproxy.cfg
sudo ufw allow 5671
sudo ufw enable
sudo service haproxy restart
```

Sample haproxy.cfg

```
global
        log /dev/log    local0
        log /dev/log    local1 notice
        chroot /var/lib/haproxy
        user haproxy
        group haproxy
        daemon

defaults
        mode tcp
        maxconn 10000
        timeout connect 5s
        timeout client 100s
        timeout server 100s

listen rabbitmq 172.25.0.30:5671
        mode tcp
        balance roundrobin
        server SSLRabbitMqCN1 172.25.0.31:5671 weight 10 check inter 2000 rise 2 fall 2
        server SSLRabbitMqCN2 172.25.0.32:5671 weight 10 check inter 2000 rise 2 fall 2
```
