btn.addEventListener("click", ()=>{
    let url = "https://localhost:7236/qr";
    url+="?text=" + textQr.value;

    fetch(url).then(res=>res.text())
    .then(text=>qr.src = "data:image/png;base64, " +text)
});