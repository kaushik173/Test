using Aspose.Words;
using Aspose.Words.Fields;
using Aspose.Words.MailMerging;

namespace LALoDep.Core.Custom.Extensions
{


    public class HandleMergeField : IFieldMergingCallback
    {
        /// <summary>
        /// This handler is called for every mail merge field found in the document,
        ///  for every record found in the data source.
        /// </summary>
        void IFieldMergingCallback.FieldMerging(FieldMergingArgs e)
        {
            if (mBuilder == null)
                mBuilder = new DocumentBuilder(e.Document);

            // We decided that we want all boolean values to be output as check box form fields.
            if (e.FieldValue is bool)
            {
                // Move the "cursor" to the current merge field.
                mBuilder.MoveToMergeField(e.FieldName);

                // It is nice to give names to check boxes. Lets generate a name such as MyField21 or so.
                string checkBoxName = string.Format("{0}{1}", e.FieldName, e.RecordIndex);

                // Insert a check box.
                mBuilder.InsertCheckBox(checkBoxName, (bool)e.FieldValue, 0);

                // Nothing else to do for this field.
                return;
            }

            // Another example, we want the Subject field to come out as text input form field.
            if (e.FieldName == "Subject")
            {
                mBuilder.MoveToMergeField(e.FieldName);
                string textInputName = string.Format("{0}{1}", e.FieldName, e.RecordIndex);
                mBuilder.InsertTextInput(textInputName, TextFormFieldType.Regular, "", (string)e.FieldValue, 0);
            }
        }

        void IFieldMergingCallback.ImageFieldMerging(ImageFieldMergingArgs args)
        {
            // Do nothing.
        }

        private DocumentBuilder mBuilder;

    }
}