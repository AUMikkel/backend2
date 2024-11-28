db = connect("mongodb://localhost:27017/assign3");

db.createCollection("logs");

db.logs.insertOne({
    message: "Logs collection initialized",
    timestamp: new Date(),
});