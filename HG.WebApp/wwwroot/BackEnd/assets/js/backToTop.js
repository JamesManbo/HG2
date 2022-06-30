let mybutton = document.getElementById("btn-back-to-top");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
  scrollFunction();
};

function scrollFunction() {
  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    mybutton.style.display = "block";
  } else {
    mybutton.style.display = "none";
  }
}
// When the user clicks on the button, scroll to the top of the document
mybutton.addEventListener("click", backToTop);

function backToTop() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
}

var $window = $(window);
function checkWidth() {
  var windowsize = $window.width();
  console.log(windowsize);

  if (windowsize <= 768) {
    if ($(".navbar-brand img").length == 0) {
      $(".navbar-brand").append(
        $("<img />").attr("src", "../public/img/logo3.png")
      );
      $(".navbar-brand img").css("width", "70%");
      $(".banner-top .title-logo").css("padding-left", "0");
    }
  } else {
    $(".navbar-brand img").css("width", "100%");
    $(".navbar-brand img").remove();
    $(".banner-top .title-logo").css("padding-left", "25px");
  }
}

checkWidth();

$(window).resize(checkWidth);
