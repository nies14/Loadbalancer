# Load Balancer

A simple load balancer with CLI implemented in C#, .NET Core, SignalR, JavaScript.

## Description

This project is a basic implementation of a load balancer using C#, .NET Core. The load balancer distributes incoming requests across multiple servers to ensure efficient utilization of resources and improved system reliability. It's also using SignalR and JavaScript to show the status of each server in real time.

## Features

- Distributes incoming requests across multiple servers.
- Health checks to monitor server status.
- Real-time server status updates using SignalR.

## Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/nies14/Loadbalancer.git
   ```


## Usage
1. **Run the load balancer using the CLI:**
   Open a terminal and type
   ```
   cd BackendServer
   dotnet build
   dotnet run --urls "http://localhost:8080" Server1
   ```

   Open another terminal

   ```
   dotnet build
   dotnet run --urls "http://localhost:8081" Server2
   ```

   
2. **Send requests to the load balancer and observe the distributed load among servers.**
   Open a terminal and type
   ```
   cd ../LoadBalancer.API
   dotnet build
   dotnet run
   ```
   Open another terminal
   ```
   curl https://localhost:90
   ```

3. **Now browse**
   ```
   https://localhost:7176/serverstatus
   ```

## Technical Details

### Round-Robin Load Balancing
The load balancer uses round-robin algorithm to evenly distribute incoming requests across the available servers. This ensures that each server receives an equal portion of the load, preventing any single server from becoming a bottleneck.

### Health Checks
Regular health checks are conducted on each server to verify its availability. If a server fails a health check, it is temporarily removed from the rotation, ensuring the load balancer does not direct requests to an unhealthy server.

### SignalR for Real-Time Updates
SignalR is used to provide real-time updates on the status of each server. This allows administrators to monitor server health and load distribution dynamically.

