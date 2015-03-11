<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilePostCtr.ascx.cs" Inherits="Web.Controls.FilePostCtr" %>

<script type="text/javascript">
function getfiledName()
{
   var a=document.getElementById("<%=filedNamelist.ClientID %>");
   return a;
}
</script>
<link href="../css/fileup.css" rel="stylesheet" type="text/css" />
<script src="../Js/Web/fileup.js" type="text/javascript"></script>
<link href="/css/jquery.loadmask.css" rel="stylesheet" />
<script src="/Js/Base/jquery.loadmask.js"></script>  
      <div class="btn btn-default btn-lg" style=" margin-top:1 em;padding:0;">   
<div id="fileUpArea" class="btn-select-pic" ></div></div>
         <div id="filetxt" class="filetxt"></div>
  <input id="filedNamelist" name="filedNamelist" type="hidden"  runat="server" value="" />
  <div id="filekey">
    <input type="hidden"  name="FileUp_Remark"  id="FileUp_Remark" value="<%=remark %>">
    <input type="hidden" name="FileUp_TypeId"  id="FileUp_TypeId" value="0" >
     
         <input  type="hidden" name="FileUp_Tk" id="FileUp_Tk"  value="">
                  <input  type="hidden" name="FileUp_Namefile" id="FileUp_Namefile"  value="">
                   <input  type="hidden" name="FileUp_Keyfile" id="FileUp_Keyfile"  value="">
                   <input  type="hidden" name="FileUp_Filesize" id="FileUp_Filesize"  value="0">
                     <input  type="hidden" name="FileUp_Id" id="FileUp_Id"  value="">
                        <input  type="hidden" name="FileUp_Type" id="FileUp_Type"  value="<%=type %>">
                  </div>
        <script type="text/jscript">
            var path = "/"//删除按钮路径
    

//   alert($("#filekey").ReturnJson())
           UPClass = "<%=Class %>", UPAutoupload = "<%=Autoupload %>", UPBf ="<%=Before %>", UPBehd = "<%=Behind %>",FileUp_localfile= "<%=UPLocal %>";

         var FSize="<%=FSize %>",FType="<%=FType %>";
           if(FSize!=""){
             acceptedFile.size=FSize

           }
           if(FType!=""){
          
              acceptedFile.ext=FType.split(',');
           }
        
         
          if(  UPClass!=""){

          $("#fileUpArea").attr("class",UPClass);
          }

             FileUp_isshow=<%=isshow %>
             if(FileUp_isshow==0){
             FileUp_isshow=false;
             }else{
             FileUp_isshow=true;
             }

   
      function  UPBefore(json){
    var tttt = $.parseJSON(json);
   ShowBaseLimitError( tttt.ErrorMS);
      }
        function UPBehind(json) {
          var tttt = $.parseJSON(json);
        ShowBaseLimitError( tttt.ErrorMS);
        }


           var FileUp_tyid="<%=typeid %>";
 $("#FileUp_TypeId").val(FileUp_tyid);





    </script>