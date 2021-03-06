﻿using Alfred.Domain.Validators;
using Alfred.Models.Artifacts;
using Alfred.Shared.Enums;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Alfred.Domain.Tests.Validators
{
    [TestFixture]
    public class ArtifactCriteriaModelValidatorTests
    {
        private ArtifactCriteriaModelValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ArtifactCriteriaModelValidator(new IdsValidator());
        }

        [Test]
        public void Should_validate_criteria_when_no_filter_was_specified()
        {
            var criteria = new ArtifactCriteriaModel
            {
                Ids = null,
                Status = null,
                Title = null,
                Type = null,
                PageSize = 20,
                Page = 1
            };

            _validator.ShouldNotHaveValidationErrorFor(x => x.Ids, criteria);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Status, criteria);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Title, criteria);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Type, criteria);
            _validator.ShouldNotHaveValidationErrorFor(x => x.PageSize, criteria);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Page, criteria);
        }

        [Test]
        public void Should_have_ids_validator_child_when_built()
        {
            _validator.ShouldHaveChildValidator(criteria => criteria.Ids, typeof(IdsValidator));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Should_have_validation_errors_when_page_value_is(int page)
        {
            _validator.ShouldHaveValidationErrorFor(c => c.Page, page);
        }

        [TestCase(50)]
        [TestCase(1)]
        public void Should_not_have_validation_errors_when_page_value_is(int page)
        {
            _validator.ShouldNotHaveValidationErrorFor(c => c.Page, page);
        }

        [TestCase(51)]
        [TestCase(-1)]
        public void Should_have_validation_errors_when_pageSize_value_is(int pageSize)
        {
            _validator.ShouldHaveValidationErrorFor(c => c.PageSize, pageSize);
        }

        [TestCase(19)]
        [TestCase(9)]
        public void Should_not_have_validation_errors_when_pageSize_value_is(int pageSize)
        {
            _validator.ShouldNotHaveValidationErrorFor(c => c.PageSize, pageSize);
        }

        [Test]
        public void Should_have_validation_errors_when_type_is_not_enum_valid()
        {
            _validator.ShouldHaveValidationErrorFor(c => c.Type, (ArtifactType?)15);
        }

        [Test]
        public void Should_not_have_validation_errors_when_role_is_null_or_enum_valid()
        {
            _validator.ShouldNotHaveValidationErrorFor(c => c.Type, (ArtifactType?)0);
        }

        [Test]
        public void Should_have_validation_errors_when_status_is_not_enum_valid()
        {
            _validator.ShouldHaveValidationErrorFor(c => c.Status, (ArtifactStatus?)15);
        }

        [Test]
        public void Should_not_have_validation_errors_when_status_is_enum_valid()
        {
            _validator.ShouldNotHaveValidationErrorFor(c => c.Status, (ArtifactStatus?)0);
        }
    }
}
