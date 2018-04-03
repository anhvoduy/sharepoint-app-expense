'use strict';

const express = require('express');
const http = require('http');
const path = require('path');

// Constants
const HOST = '0.0.0.0';
const PORT = 8080;

const server = express();
server.set('host', HOST);
server.set('port', PORT);

// register api
server.get('/api', (req, res) => {
  res.send('Hello docker & nodejs web app ...');
});

server.get('/api/product', (req, res) => {
  res.send('Dell Lattitude -> IBM Thinkpad T60 -> Sony Vaio -> MacBook Pro');
});

// register web app
var src = path.join(__dirname, 'src');
server.use('/', express.static(src, { index: 'index.html' }));
server.use('/index', express.static(src, { index: 'index.html' }));
server.use('/alternate', express.static(src, { index: 'alternate.html' }));

// export
module.exports = server;
