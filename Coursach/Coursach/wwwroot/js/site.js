function changeButtonText(id, text1, text2) {
    console.log(id);
    let element=document.getElementById(id);
    if(element!=null)
        element.textContent = element.textContent ==text1?text2:text1;
}