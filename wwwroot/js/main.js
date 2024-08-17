(function ($) {
    "use strict";

    /*------------------------------------
        Sticky Menu 
    --------------------------------------*/
    var windows = $(window);
    var stick = $(".header-sticky");
    windows.on('scroll', function () {
        var scroll = windows.scrollTop();
        if (scroll < 5) {
            stick.removeClass("sticky");
        } else {
            stick.addClass("sticky");
        }
    });
    /*------------------------------------
        jQuery MeanMenu 
    --------------------------------------*/
    $('.main-menu nav').meanmenu({
        meanScreenWidth: "767",
        meanMenuContainer: '.mobile-menu'
    });

    // course search
    $(document).ready(function () {
        $('#searchForm').submit(function (event) {
            event.preventDefault();

            var searchText = $('input[name="text"]').val();

            $.ajax({
                url: '@Url.Action("Search", "Course")',
                type: 'GET',
                data: { text: searchText },
                success: function (result) {
                    $('#coursesArea').html(result);
                },
                error: function () {
                    $('#coursesArea').html('<p>An error occurred while fetching the results.</p>');
                }
            });
        });

        // course search

        /* last  2 li child add class */
        $('ul.menu>li').slice(-2).addClass('last-elements');
        /*------------------------------------
            Owl Carousel
        --------------------------------------*/
        $('.slider-owl').owlCarousel({
            loop: true,
            nav: true,
            animateOut: 'fadeOut',
            animateIn: 'fadeIn',
            smartSpeed: 2500,
            navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });

        $('.partner-owl').owlCarousel({
            loop: true,
            nav: true,
            navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 3
                },
                1000: {
                    items: 5
                }
            }
        });

        $('.testimonial-owl').owlCarousel({
            loop: true,
            nav: true,
            navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });
        /*------------------------------------
            Video Player
        --------------------------------------*/
        $('.video-popup').magnificPopup({
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            zoom: {
                enabled: true,
            }
        });

        $('.image-popup').magnificPopup({
            type: 'image',
            gallery: {
                enabled: true
            }
        });
        /*----------------------------
            Wow js active
        ------------------------------ */
        new WOW().init();
        /*------------------------------------
            Scrollup
        --------------------------------------*/
        $.scrollUp({
            scrollText: '<i class="fa fa-angle-up"></i>',
            easingType: 'linear',
            scrollSpeed: 900,
            animation: 'fade'
        });
        /*------------------------------------
            Nicescroll
        --------------------------------------*/
        $('body').scrollspy({
            target: '.navbar-collapse',
            offset: 95
        });
        $(".notice-left").niceScroll({
            cursorcolor: "#EC1C23",
            cursorborder: "0px solid #fff",
            autohidemode: false,
        });
    });
}(jQuery));
function formatPhoneNumber(number) {
    number = number.toString().replace(/\D/g, '');
    if (number.length === 8) {
        return `+1(${number.substring(0, 3)})${number.substring(3, 6)}-${number.substring(6)}`;
    } else {
        return number;
    }
}

function initializePhoneNumber() {
    var phoneNumberElement = document.getElementById('phoneNumber');
    if (phoneNumberElement) {
        var number = phoneNumberElement.getAttribute('data-number');
        phoneNumberElement.textContent = formatPhoneNumber(number);
    }
}

document.addEventListener('DOMContentLoaded', initializePhoneNumber);
