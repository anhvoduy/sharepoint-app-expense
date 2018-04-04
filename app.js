const http = require('http');
const server = require('./server');

/* ----------- Start Server -----------*/
http.createServer(server).listen(server.get('port'), function () {
    console.log('eCoffee WebSite is running on port:' + server.get('port'));
});