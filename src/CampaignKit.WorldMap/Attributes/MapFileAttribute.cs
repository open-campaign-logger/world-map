// Copyright 2017-2020 Jochen Linnemann, Cory Gill
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace CampaignKit.WorldMap.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    /// <summary>
    /// This custom validation attribute decorates a model field with the following jQuery validation attributes:
    /// 
    /// - data-val-mapfilesize and
    /// - data-val-mapfiletye
    /// 
    /// Upon submission the corresponding jQuery validation functions in worldmap-validation.js are run.
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    public class MapFileAttribute : ValidationAttribute, IClientModelValidator
    {

        #region "Attribute Properties"

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        public long MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the allowable file extensions.
        /// This should be in comma delimited format without periods.
        /// 
        /// example for images: png,gif,jpg,bmp
        /// 
        /// </summary>
        /// <value>
        /// The extensions.
        /// </value>
        public String Extensions { get; set; }

        #endregion

        #region "Server Side Validation"

        /// <summary>
        /// Server side validation always returns true.  We are really implementing this item
        /// as client-side only to verify file size and file extension before attempting to upload.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="System.ComponentModel.DataAnnotations.ValidationResult"></see> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The only supportable object type is IFormFile
            IFormFile file = (IFormFile)value;

            // Check file size
            if (file.Length > MaxLength)
            {
                var maxMB = Math.Floor((double)MaxLength / 1024 / 1024);

                return new ValidationResult($"Invalid file size. File must be less that {maxMB} MB.");
            }

            // Check file type
            var extensionList = Extensions.Split(",");
            var validExtension = false;
            foreach (String ext in extensionList)
            {
                if (file.FileName.ToLower().EndsWith("." + ext.ToLower()))
                {
                    validExtension = true;
                }
            }
            if (!validExtension)
            {
                return new ValidationResult($"Invalid file type. File type must be one of the following: {Extensions}.");
            }

            return ValidationResult.Success;
        }

        #endregion

        #region "Client Side Validation"

        /// <summary>
        /// Adds the validation tags to the field so 
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            var maxMB = Math.Floor((double)MaxLength / 1024 / 1024);
            context.Attributes.Add("data-val-mapfilesize", $"Invalid file size. File must be less that {maxMB} MB.");
            context.Attributes.Add("data-val-mapfiletype", $"Invalid file type. File type must be one of the following: {Extensions}.");
        }

        #endregion

    }
}
