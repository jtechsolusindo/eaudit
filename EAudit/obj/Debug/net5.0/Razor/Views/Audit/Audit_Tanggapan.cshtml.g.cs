#pragma checksum "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7a487f10aa55e50b652a80d3ad82d9923e696129"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Audit_Audit_Tanggapan), @"mvc.1.0.view", @"/Views/Audit/Audit_Tanggapan.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\_ViewImports.cshtml"
using EAudit;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\_ViewImports.cshtml"
using EAudit.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a487f10aa55e50b652a80d3ad82d9923e696129", @"/Views/Audit/Audit_Tanggapan.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e5da2ec1e7949fc732b1c9b7d80c42c8f787248c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Audit_Audit_Tanggapan : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/_PageHeaderTitle.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "~/Views/Shared/Filter/Filter_1Field.cshtml", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formEditTanggapan"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formVerifikasi"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
  
    ViewData["placeHolderFilter"] = "Pencarian Tanggapan Audit";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "7a487f10aa55e50b652a80d3ad82d9923e6961295777", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n<div class=\"content\">\r\n    <div class=\"container-fluid\">\r\n\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "7a487f10aa55e50b652a80d3ad82d9923e6961296975", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
#nullable restore
#line 10 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.ViewData = ViewData;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("view-data", __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.ViewData, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

        <div class=""row"">
            <div class=""col-md-12"">
                <div class=""card card-default card-sm"">

                    <div class=""card-body card-body-sm"">
                        <table class=""table table-bordered table-striped table-responsive-sm"" style=""width:100%"" id=""grid"">
                            <thead>
                                 <tr>
                                    <th> No </th>
                                    <th> Akar Masalah </th>
                                    <th> Analisis Akar Masalah </th>
                                    <th> Tindakan Koreksi </th>
                                    <th> Tindakan Korektif</th>
                                    <th> Bukti </th>
                                    <th> STATUS </th>
                                    <th> VERIFIKASI </th>
                                    <th> AKSI </th>
                                </tr>
                            </thead>
                        </ta");
            WriteLiteral(@"ble>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class=""modal fade"" role=""dialog"" id=""modalTanggapan"" data-keyboard=""false"">
    <div class=""modal-dialog modal-dialog-centered"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"" id=""modalHeader"">Default Modal</h4>
            </div>
             ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a487f10aa55e50b652a80d3ad82d9923e69612910080", async() => {
                WriteLiteral("\r\n            <div class=\"modal-body\">\r\n               \r\n                    ");
#nullable restore
#line 49 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                     <input type=""hidden"" name=""id_edit"" class=""form-control form-control-sm"" id=""tgp_id_edit"">
                    <input type=""hidden"" name=""id_temuan"" class=""form-control form-control-sm"" id=""tgp_id_temuan"">
                   
                    <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Akar Masalah</label>
                        <div class=""col-sm-9"">
                            <select style=""width:100%;"" class=""form-control select2 select2bs4"" id=""input_akar_masalah"" name=""akar_masalah"" required>
                              ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a487f10aa55e50b652a80d3ad82d9923e69612911365", async() => {
                    WriteLiteral("-");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                            </select>
                        </div>
                    </div>

                      
                    <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Analisis akar masalah </label>
                        <div class=""col-sm-9"">
                       <textarea class=""form-control"" id=""input_analisis"" name=""analisis"" required></textarea>
                        </div>
                    </div>
                    
                      <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Tindakan koreksi </label>
                        <div class=""col-sm-9"">
                       <textarea class=""form-control"" id=""input_koreksi"" name=""koreksi"" required></textarea>
                        </div>
                    </div>

                    <div class=""form-group");
                WriteLiteral(@" row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Tindakan korektif </label>
                        <div class=""col-sm-9"">
                       <textarea class=""form-control"" id=""input_korektif"" name=""korektif"" required></textarea>
                        </div>
                    </div>

                      <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm""> Pilih Jenis File</label>
                        <div class=""col-sm-9 pt-1"">
                             <div class=""form-check form-check-inline"">
                                <input class=""form-check-input"" id=""radioDokumen"" type=""radio"" name=""jenis"" value=""doc"" checked required>
                                <label class=""form-check-label"" for=""radio dokumen""> Dokumen/Gambar </label>
                            </div>
                            <div class=""form");
                WriteLiteral(@"-check form-check-inline"">
                                <input class=""form-check-input"" id=""radioVideo"" type=""radio"" name=""jenis"" value=""vid"" required>
                                <label class=""form-check-label"" for=""radio video""> Link Dokumen </label>
                            </div>
                        </div>
                    </div>
                      <div class=""form-group row form-group-sm div_input_file"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">File</label>
                        <div class=""col-sm-9"">
                            <input class=""form-control-file"" type=""file"" id=""input_file"" name=""file"" placeholder=""Unggah file..."" onchange=""getExt(event, 'create')"" />
                            <input id='input_tipe_file' type='hidden' name='tipe_file'>
                        </div>
                    </div>

                     <div class=""form-group row form-group-sm"">
                        <label for=");
                WriteLiteral(@"""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Nama File </label>
                        <div class=""col-sm-9"">
                            <input class=""form-control"" type=""text"" id=""input_nama_file"" name=""nama_file"" required />
                        </div>
                    </div>
                    
                     <div class=""form-group row form-group-sm div_input_link"" style=""display:none"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Link </label>
                        <div class=""col-sm-9"">
                            <input class=""form-control"" type=""text"" id=""input_link"" name=""link"" />
                        </div>
                    </div>                
            </div>

            <div class=""modal-footer"">
                <button type=""submit"" class=""btn btn-success btn-sm"">Simpan</button>
                <button type=""button"" id=""btnCancel"" class=""btn btn-default btn-sm"">Batal</button>");
                WriteLiteral("\r\n            </div>\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>


<div class=""modal fade"" role=""dialog"" id=""modalVerifikasi"" data-keyboard=""false"">
    <div class=""modal-dialog modal-dialog-centered"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"" id=""modalHeader"">Default Modal</h4>
            </div>
             ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a487f10aa55e50b652a80d3ad82d9923e69612918589", async() => {
                WriteLiteral("\r\n            <div class=\"modal-body\">\r\n               \r\n                    ");
#nullable restore
#line 141 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                    <input type=""hidden"" name=""id_tanggapan"" class=""form-control form-control-sm"" id=""id_tanggapan"">
                   
                 
                      <div class=""form-group row form-group-sm"">
                        <label  class=""col-sm-3 col-form-label col-form-label-sm""> Konfirmasi</label>
                        <div class=""col-sm-9 pt-1"">
                             <div class=""form-check form-check-inline"">
                                <input class=""form-check-input"" type=""radio"" name=""konfirmasi"" value=""Setuju"" required>
                                <label class=""form-check-label"">Setuju </label>
                            </div>
                            <div class=""form-check form-check-inline"">
                                <input class=""form-check-input""  type=""radio"" name=""konfirmasi"" value=""Tidak"" required>
                                <label class=""form-check-label""> Tidak Setuju </label>
                            </div>
              ");
                WriteLiteral(@"          </div>
                    </div>
                      
                    <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Catatan </label>
                        <div class=""col-sm-9"">
                       <textarea class=""form-control"" id=""input_catatan"" name=""catatan""></textarea>
                        </div>
                    </div>
                    
                      <div class=""form-group row form-group-sm"">
                        <label for=""txtUnitAuditee"" class=""col-sm-3 col-form-label col-form-label-sm"">Uraian </label>
                        <div class=""col-sm-9"">
                       <textarea class=""form-control"" id=""input_uraian"" name=""uraian""></textarea>
                        </div>
                    </div>


                    
            </div>

            <div class=""modal-footer"">
                <button type=""submit"" class=""btn btn-succes");
                WriteLiteral("s btn-sm\">Verifikasi</button>\r\n                <button type=\"button\" id=\"btnCancel\" class=\"btn btn-default btn-sm\">Batal</button>\r\n            </div>\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n        <!-- /.modal-content -->\r\n    </div>\r\n    <!-- /.modal-dialog -->\r\n</div>\r\n\r\n\r\n\r\n");
            DefineSection("JavaScript", async() => {
                WriteLiteral("\r\n<script type=\"text/javascript\">\r\n    let role=\"");
#nullable restore
#line 193 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
         Write(ViewData["Role"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n    let id_auditor=\"");
#nullable restore
#line 194 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
               Write(ViewData["IdAuditor"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n</script>\r\n<script type=\"text/javascript\"");
                BeginWriteAttribute("src", " src=\"", 9822, "\"", 9874, 1);
#nullable restore
#line 196 "C:\xampp\htdocs\dotnet\github\eaudit\EAudit\Views\Audit\Audit_Tanggapan.cshtml"
WriteAttributeValue("", 9828, Url.Content("/js/modules/audit_tanggapan.js"), 9828, 46, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("></script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591