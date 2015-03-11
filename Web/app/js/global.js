$(window).load(function(){
	equalHeight();
	
}).resize(function(){
	equalHeight();
});

$(document).ready(function(){
	var $body = $('body'),
		$header = $('header'),
		$appLeft = $('.app-left'),
		$emailLi = $('.am-list > li'),
		$appDataGrid = $('#app-data-grid'),
		$appStatCircle = $('#app-stat-circle'),
		$appSuperAdd = $('#app-super-add');
		
	$header.on('click', '.am-icon-search', function(){
		$header.find('.app-search-top').toggle();
	});	

	if ($body.hasClass('p-dashboard-single')){
		$('table.sent').show();
		
		$appLeft.on('click.single.dashboard', '.am-list > li', function(){
			var $this = $(this);
			$emailLi.removeClass('active');
			$this.addClass('active');
			// TODO: load contents for right column by ajax
		});
		
		$appStatCircle.on('click', 'a', function(){
			var status = $(this).data('status');
			$appDataGrid.find('table').hide();
			$appDataGrid.find('table.'+ status).show();
			return false;
		});
		
		$appSuperAdd.on('click', '.am-icon-plus-circle', function(){
			return;
			var $gift = $appSuperAdd.find('.gift'),
				isVisible = $gift.is(':visible');
			$gift[(isVisible)?'fadeOut':'fadeIn'](function(){
				if (!isVisible){
					$(this).css('display', 'inline-block');
				}
			});
		})
	}
});