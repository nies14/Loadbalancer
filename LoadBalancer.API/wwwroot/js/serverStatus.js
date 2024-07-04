const connection = new signalR.HubConnectionBuilder()
    .withUrl("/serverstatushub")
    .build();

connection.on("UpdateServerStatus", (host, port, isHealthy) => {
    const serverStatusList = document.getElementById("serverStatusList");
    const serverId = `${host}:${port}`;

    // Check if the server entry already exists
    let existingServerStatus = document.getElementById(serverId);

    if (existingServerStatus) {
        // Update the status of the existing server entry
        existingServerStatus.textContent = `Server: ${host}:${port} - Status: ${isHealthy ? "Healthy" : "Unhealthy"}`;
    } else {
        // Create a new server entry
        const serverStatus = document.createElement("li");
        serverStatus.id = serverId;
        serverStatus.textContent = `Server: ${host}:${port} - Status: ${isHealthy ? "Healthy" : "Unhealthy"}`;
        serverStatusList.appendChild(serverStatus);
    }
});

connection.start()
    .then(() => console.log('SignalR connection established.'))
    .catch(err => console.error('SignalR connection error: ', err));