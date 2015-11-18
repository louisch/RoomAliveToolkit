using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace RoomAliveToolkit {

    /// <summary>
    /// Loads the ProjectorForms from an ensemble file.
    /// </summary>
    /// <remarks>
    /// Also allows accessing those forms via the Forms accessor.
    /// </remarks>
    public class ProjectorFormLoader {

        const bool FULLSCREEN_ENABLED = true;

        /// <summary>
        /// Projector data loaded from ensemble file, including projection matrix etc.
        /// </summary>
        public List<ProjectorForm> Forms { get; private set; }

        public ProjectorFormLoader(String path) {
            Forms = new List<ProjectorForm>();

            // load ensemble.xml
            string directory = Path.GetDirectoryName(path);
            var ensemble = ProjectorCameraEnsemble.FromFile(path);

            // create d3d device
            var factory = new Factory1();
            var adapter = factory.Adapters[0];

            // When using DeviceCreationFlags.Debug on Windows 10, ensure that "Graphics Tools" are installed via Settings/System/Apps & features/Manage optional features.
            // Also, when debugging in VS, "Enable native code debugging" must be selected on the project.
            var device = new SharpDX.Direct3D11.Device(adapter, DeviceCreationFlags.None);

            Object renderLock = new Object();

            // create a form for each projector
            foreach (var projector in ensemble.projectors) {
                var form = new ProjectorForm(factory, device, renderLock, projector);
                form.FullScreen = FULLSCREEN_ENABLED; // TODO: fix this so can be called after Show
                form.Show();
                Forms.Add(form);
            }
        }
    }
}
