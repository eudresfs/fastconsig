$(function () {
    //all hover and click logic for buttons
    $(".fg-button:not(.ui-state-disabled)")
		.hover(
			function () {
			    $(this).addClass("ui-state-hover");
			},
			function () {
			    $(this).removeClass("ui-state-hover");
			}
		)
		.mousedown(function () {
		    $(this).parents('.fg-buttonset-single:first').find(".fg-button.ui-state-active").removeClass("ui-state-active");
		    if ($(this).is('.ui-state-active.fg-button-toggleable, .fg-buttonset-multi .ui-state-active')) { $(this).removeClass("ui-state-active"); }
		    else { $(this).addClass("ui-state-active"); }
		})
		.mouseup(function () {
		    if (!$(this).is('.fg-button-toggleable, .fg-buttonset-single .fg-button,  .fg-buttonset-multi .fg-button')) {
		        $(this).removeClass("ui-state-active");
		    }
		});
});



function messagebox(message, title, btnName) {
    $.blockUI.defaults = {
        themedCSS: {
            width: '30%',
            top: '10%',
            left: '35%'
        },
        overlayCSS: {
            backgroundColor: '#000',
            opacity: 0.6,
            cursor: 'default'
        },
        // styles applied when using $.growlUI
	growlCSS: {
		width:  	'350px',
		top:		'10px',
		left:   	'',
		right:  	'10px',
		border: 	'none',
		padding:	'5px',
		opacity:	0.6,
		cursor: 	'default',
		color:		'#fff',
		backgroundColor: '#000',
		'-webkit-border-radius': '10px',
		'-moz-border-radius':	 '10px'
	},
	
    	baseZ: 1000,
    	centerX: true, // <-- only effects element blocking (page block controlled via css above)
    	centerY: true,
    	allowBodyStretch: true,
    	fadeIn:  200,
    	fadeOut:  400,
    	timeout: 0,
    	showOverlay: true,
    	applyPlatformOpacityRules: true,
    	onBlock: null,
    	onUnblock: null,	
    	quirksmodeOffsetHack: 4
        
    };
    $.blockUI({
        theme: true,
        title: title,
        message: '<div align="justify"><p>' + message + '</p> </div><input style="float:right" class="fg-button ui-state-default ui-corner-all" type="button" id="close" value=' + btnName + ' />'
    });

    $('#close').click(function () {
        $.unblockUI();
        return false;
    });
}


function growlMessagebox(title,message) {
    $(document).ready(function () {
        $.growlUI(title.toString(), message.toString());
    });
}

    