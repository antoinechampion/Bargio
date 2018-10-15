alert("test");

$(document).ready(function(){
    $('#checkbox_optional').change(function(){
        
        if(this.checked)
            $('#mdp').fadeIn('slow');
        else
            $('#mdp').fadeOut('slow');

    });
});