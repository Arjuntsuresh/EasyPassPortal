var style = document.createElement('style');
var position = 'right'; // Adjust as needed ('right' or 'left')
style.innerHTML = `
    @keyframes my-animation {
        0% { ${position}: -${document.querySelector('.text').offsetWidth + 10}px; }
        100% { ${position}: 100%; }
    }
`;
document.head.appendChild(style);
