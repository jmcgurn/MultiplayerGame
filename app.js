var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require("shortid");
var mongoose = require('mongoose');

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PlayerSchema = new Schema({
    name:{
        type:String
    },
    score:{
        type:Number
    }
});

//NOTES: this is not fully functional there are some issues
//when new client connect other players scores wont update to
//there actual score until they move once. Also the mongoose
//db constantly puts records everytime you move. I tried to make
//it work so it didnt actually save if the player already existed.
//I ran out of time but i believe this meets requirements for part 1.


mongoose.model('player', PlayerSchema);

//connect to mongoose
mongoose.connect("mongodb://localhost:27017/playerStorage" ,{
    useNewUrlParser:true,
    useUnifiedTopology:true
}).then(function(){
    console.log('mongodb connected');
}).catch(function(err){
    console.log(err);
});

var Players = mongoose.model('player');

console.log(shortid.generate());

console.log("SERVER CONNECTED");
var players = [];


io.on('connection', function(socket){
    console.log('client connected');

    var thisClientId = shortid.generate();
    players.push(thisClientId);


//spawn newly joined characters
    socket.broadcast.emit('spawn', {id:thisClientId, score:100});
    //request logged in players position
    socket.broadcast.emit('requestPosition');    
    socket.broadcast.emit('requestScore');
    
    players.forEach(function(playerId){

        if(playerId == thisClientId)
        {
            return;
        }
         //spawns player character
         socket.emit('spawn',{id:playerId});
         console.log('spawning player of id: ', playerId);
    })
    for(var i = 0; i<players; i++)
    {
       
    }

    socket.on('updatePosition', function(data){
        data.id = thisClientId;
        socket.broadcast.emit('updatePosition', data);
    });

    socket.on('updateScore', function(data){
        data.id = thisClientId;

        console.log(JSON.stringify(data));
        socket.broadcast.emit('updateScore', data);

    });

    socket.on('move', function(data){
        
        data.id = thisClientId;
        Players.findOne({
            name:data["name"]
        }).then(function(player){
            //tried to make it so it didnt do this on every click but 
            //ran out of time and couldnt figure it out
                var newUser = {
                    name:data["id"],
                    score:data["score"]           
               }
        
                new Players(newUser).save();
                socket.broadcast.emit('move', data);
            
   
            }).catch(function(player){            
            });       
    });

    socket.on('disconnect', function(){
        console.log("player disconnected");

        players.splice(players.indexOf(thisClientId), 1);
        socket.broadcast.emit('disconnected', {id:thisClientId});
    })
});