$(document).ready(function() {
  $('.carousel').carousel();
  $('.advert__curent-img').eq(0).attr('src', $('.advert__img').eq(0).attr('src'));
  $('.advert__img').removeClass('advert__photo--now');
  $('.advert__img').eq(0).addClass('advert__photo--now');
  $('.advert__img').click(function() {
    $('.advert__img').removeClass('advert__photo--now');
    $(this).addClass('advert__photo--now');
    $('.advert__curent-img').eq(0).attr('src', $(this).attr('src'));
  });
});
