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

function switchStats(tab) {
    const data = window._statsData;
    if (!data) return;

    document.querySelectorAll('.stat-tab').forEach(btn => {
        btn.className = 'stat-tab px-6 sm:px-10 py-2 text-gray-500 dark:text-gray-400 text-xs sm:text-sm font-semibold hover:text-cyberMain dark:hover:text-white transition-all rounded-lg';
    });

    const activeBtn = document.getElementById('tab-' + tab);
    activeBtn.className = 'stat-tab px-6 sm:px-10 py-2 bg-lightGreen text-white rounded-lg text-xs sm:text-sm font-semibold shadow-sm';

    if (tab === 'visitor') {
        document.getElementById('stat-daily').textContent = data.visitorDaily;
        document.getElementById('stat-weekly').textContent = data.visitorWeekly;
        document.getElementById('stat-monthly').textContent = data.visitorMonthly;
        document.getElementById('stat-total').textContent = data.visitorTotal;
    } else {
        document.getElementById('stat-daily').textContent = data.vacancyDaily;
        document.getElementById('stat-weekly').textContent = data.vacancyWeekly;
        document.getElementById('stat-monthly').textContent = data.vacancyMonthly;
        document.getElementById('stat-total').textContent = data.vacancyTotal;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    initSlider();
});
