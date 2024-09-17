import Toastify from "../node_modules/toastify-js";

const button = document.getElementById("notifyBtn");
button.addEventListener("click", () => {
  Notify("Hello, I am a notification");
});
function Notify(message) {
  Toastify({
    text: message,
    duration: 3000,
    newWindow: true,
    close: true,
    gravity: "top",
    position: "right",
    backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
    stopOnFocus: true,
  }).showToast();
}
