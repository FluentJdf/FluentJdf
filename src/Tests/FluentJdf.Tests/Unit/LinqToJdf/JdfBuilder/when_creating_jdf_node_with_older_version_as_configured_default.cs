using FluentJdf.Configuration;
using FluentJdf.LinqToJdf;
using Machine.Specifications;

namespace FluentJdf.Tests.Unit.LinqToJdf.JdfBuilder {
    [Subject("Highly fluent JDF interface")]
    public class when_creating_jdf_node_with_older_version_as_configured_default {
        static Ticket ticket;

        Establish context = () => Library.Settings.WithJdfAuthoringSettings().JdfVersion(JdfVersion.Version_1_1);

        Because of = () => ticket = Ticket.CreateProcess(ProcessType.Cutting, ProcessType.Creasing, ProcessType.AssetListCreation).Ticket;

        It should_have_version_attribute_with_default_value = () => ticket.Root.GetVersion().ShouldEqual("1.1");
    }
}