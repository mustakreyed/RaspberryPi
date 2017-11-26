$(document).ready(function(){
                  var i=1;
                 $("#add_row").click(function(){
                  $('#addr'+i).html("<td>"+ (i+1) +"</td><td><input name='pin"+i+"' type='text' placeholder='Pin' class='form-control input-md'  /> </td><td><input  name='name"+i+"' type='text' placeholder='Device Name'  class='form-control input-md'></td><td><input  name='location"+i+"' type='text' placeholder='Location'  class='form-control input-md'></td>");

                  $('#tab_logic').append('<tr id="addr'+(i+1)+'"></tr>');
                  i++; 
              });
                 $("#delete_row").click(function(){
                     if(i>1){
                     $("#addr"+(i-1)).html('');
                     i--;
                     }
    });

});