function equalHeight(){
	var objLeft = $('.app-left'),
		objRight = $('.app-right'),
		accordion = objLeft.find('.am-accordion'),
		isNicescroll = accordion.getNiceScroll(),
		hHeader = $('header').outerHeight(),
		hLeft = objLeft.outerHeight(),
		hRight = objRight.outerHeight(),
		hBody = $(document).height() - hHeader;
		
	accordion.height($(window.top).height() - hHeader);
	accordion.niceScroll({
		cursorwidth: '8px',
		horizrailenabled: false
	});
	//accordion.getNiceScroll().resize();
		
	objLeft.css('height','');

	if (hRight < hBody){ hRight = hBody; }
	objLeft.height(hRight);
}