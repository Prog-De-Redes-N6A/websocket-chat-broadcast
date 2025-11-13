const socket = new WebSocket("ws://localhost:5247/ws");

socket.addEventListener("open", () => {
  console.log("¡Conexión establecida!");
});

socket.addEventListener("message", (event) => {
  const data = JSON.parse(event.data);
  switch (data.MessageType) {
    case "SendMessage":
      addMessageToChat(data.Data.message);
      break;
    default:
      console.log("Tipo de mensaje desconocido:", data.MessageType);
  }
});

socket.addEventListener("error", (event) => {
  console.error("Error de WebSocket:", event);
});

socket.addEventListener("close", (event) => {
  if (event.wasClean) {
    console.log(
      `Conexión cerrada limpiamente, código=${event.code}, razón=${event.reason}`
    );
  } else {
    console.error("La conexión se cerró de forma inesperada.");
  }
});

function sendMessage() {
  const inputMessage = document.getElementById("message");
  if (socket.readyState === WebSocket.OPEN) {
    const payload = {
      MessageType: "SendMessage",
      Data: {
        message: inputMessage.value,
      },
    };
    socket.send(JSON.stringify(payload));
    inputMessage.value = "";
  } else {
    console.log("El socket no está abierto, no se puede enviar el mensaje.");
  }
}

// Acciones del usuario
const buttonSend = document.getElementById("send");
buttonSend.addEventListener("click", sendMessage);

// Acciones de WebSocket
function addMessageToChat(message) {
  const chatBox = document.getElementById("chat-box");
  const messageElement = document.createElement("div");
  messageElement.textContent = message;
  chatBox.appendChild(messageElement);
}
