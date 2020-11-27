$(function() {
    var swiper1 = new Swiper('.swiper1', {
        effect: 'coverflow',
        loop: true,
        centeredSlides: true,
        slidesPerView: 2,
        keyboardControl: true,
        mousewheelControl: true,
        lazyLoading: true,
        preventClicks: false,
        preventClicksPropagation: false,
        lazyLoadingInPrevNext: true,
        navigation: {
            nextEl: '.sbn',
            prevEl: '.sbp',
        },
        coverflowEffect: {
            rotate: 0,
            stretch: 3,
            depth: 100,
            modifier: 5,
            slideShadows : false,
        },
        breakpoints: {
            1023: {
                slidesPerView: 1,
                spaceBetween: 50,
                effect: false,

            }
        },
    });


    var swiper2 = new Swiper('.swiper2', {
        navigation: {
            nextEl: '.swn',
            prevEl: '.swp',
        },
        loop: true
    });

    // Get Items To Set CountDown

    let daysItem = document.querySelector("#days");
    let hoursItem = document.querySelector("#hours");
    let minItem = document.querySelector("#min");
    let secItem = document.querySelector("#sec");


// function to countdown

    let countDown = () => {
        let futureDate = new Date("1 Jan 2021");
        let currentDate = new Date();

        let myDate = futureDate - currentDate;

        let days = Math.floor(myDate / 1000 / 60 / 60 / 24);

        let hours = Math.floor(myDate / 1000 / 60 / 60 ) % 24;


        let sec = Math.floor(myDate / 1000 ) % 60;

        daysItem.innerHTML = days;

        hoursItem.innerHTML = hours;


        secItem.innerHTML = sec;

    }

    countDown()

    setInterval(countDown, 1000)

    $('#phone-id, #phone, #tel').mask('+7(999) 999-99-99',{placeholder: "Телефон нөміріңіз"});
    $('#tel').mask('+7(999) 999-99-99',{placeholder: "+7 ( ___ ) - ___ -__ - __"});

    $("#form").submit(function() { //Change
        var th = $(this);
        $.ajax({
        	type: "POST",
        	url: "/mail.php", //Change
        	data: th.serialize()
        }).done(function() {
        	alert("Спасибо, мы с вами свяжемся!");
        	setTimeout(function() {
        		// Done Functions
        		th.trigger("reset");
        	}, 1000);
        });
        return false;
    });
});
