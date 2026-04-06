
function initSlider() {
    const slides = document.querySelectorAll('.banner-slide');
    if (slides.length === 0) return;
    let currentPositions = ['slide-left', 'slide-right', 'slide-main'];
    setInterval(() => {
        currentPositions.unshift(currentPositions.pop());
        slides.forEach((slide, index) => {
            slide.className = 'banner-slide ' + currentPositions[index];
        });
    }, 3000);
}


document.addEventListener('DOMContentLoaded', () => {
    initSlider();
});