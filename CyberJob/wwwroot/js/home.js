function initSlider() {
    const swiperEl = document.querySelector('.banner-swiper');
    const slides = swiperEl.querySelectorAll('.swiper-slide');
    if (slides.length === 0) return;

    let current = 0;
    const total = slides.length;

    function applyClasses(activeIndex) {
        slides.forEach(s => s.classList.remove('slide-main', 'slide-left', 'slide-right'));

        slides.forEach((slide, i) => {
            let diff = (i - activeIndex + total) % total;
            if (diff > total / 2) diff -= total;

            if (diff === 0) slide.classList.add('slide-main');
            else if (diff === -1 || diff === total - 1) slide.classList.add('slide-left');
            else if (diff === 1 || diff === -(total - 1)) slide.classList.add('slide-right');
        });

        // Update pagination dot
        const dots = paginationEl?.querySelectorAll('.swiper-pagination-bullet');
        if (dots) dots.forEach((d, i) => d.classList.toggle('swiper-pagination-bullet-active', i === activeIndex));
    }

    // Pagination dots
    const paginationEl = swiperEl.querySelector('.swiper-pagination');
    if (paginationEl) {
        slides.forEach((_, i) => {
            const dot = document.createElement('span');
            dot.className = 'swiper-pagination-bullet' + (i === 0 ? ' swiper-pagination-bullet-active' : '');
            dot.addEventListener('click', () => {
                current = i;
                applyClasses(i);
            });
            paginationEl.appendChild(dot);
        });
    }

    applyClasses(0);

    setInterval(() => {
        current = (current + 1) % total;
        applyClasses(current);
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
