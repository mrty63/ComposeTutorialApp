
function StyleIndex() {

    console.log("hello world");
    document.body.style.backgroundImage = "url(images/train.jpg)";
    //alterFooter();
}

function alterFooter() {
    const el = document.getElementById("footer");
    console.log(el)
    el.classList.add("HighlightWhiteText");

}

