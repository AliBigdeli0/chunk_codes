const PORT = 3000;
const express = require("express");
const app = express();


app.use(express.static(__dirname + "/"));

app.get('/', (req, res) => {
    res.redirect('/index.html');
});
app.listen(PORT, () => {
    console.log(`Server started on ${PORT}`);
});