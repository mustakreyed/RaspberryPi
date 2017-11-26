$('.owl-carousel').owlCarousel({
    loop:true,
    margin:0,
    nav:false,
    dots:false,
    autoplay:true,
    autoplayHoverPause:true,
    smartSpeed:500,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:2
        },
        1000:{
            items:3
        }
    }
})



function adjust_textarea(h) {
    h.style.height = "20px";
    h.style.height = (h.scrollHeight)+"px";
}

//inserting new row in the table

$(document).ready(function(){
    $(".nav-tabs a").click(function(){
    $(this).tab('show');
    });
});


//typed js

//document.addEventListener('DOMContentLoaded', function(){
//		Typed.new('#typed', {
//			stringsElement: document.getElementById('typed-strings'),
//            loop: true
//		});
//	});