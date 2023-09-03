import { useEffect, useState } from "react";
import PictureWidgetDropzone from "./PictureWidgetDropzone";
import PictureWidgetCropper from "./PictureWidgetCropper";

interface Props {
  uploadPicture: (file: Blob) => void;
}

const PictureUploadWidget = ({ uploadPicture }: Props) => {
  const [files, setFiles] = useState<any>([]);
  const [cropper, setCropper] = useState<Cropper>();

  const onCrop = () => {
    if (cropper) {
      cropper.getCroppedCanvas().toBlob((blob) => uploadPicture(blob!));
    }
  };

  useEffect(() => {
    return () => {
      files.forEach((file: any) => URL.revokeObjectURL(file.preview));
    };
  }, [files]);

  return (
    <div>
      <div>
        <h2 className="text-lg mb-2">Step One - Add Picture</h2>
        <PictureWidgetDropzone setFiles={setFiles} />
      </div>
      <div>
        <h2 className="text-lg my-2">Step Two - Resize Picture</h2>
        {files && files.length > 0 && (
          <PictureWidgetCropper
            setCropper={setCropper}
            imagePreview={files[0].preview}
          />
        )}
      </div>
      <div>
        <h2 className="text-lg my-2">Step Three - Preview and upload</h2>
        {files && files.length > 0 && (
          <>
            <div
              className="img-preview mx-auto"
              style={{ minHeight: 200, overflow: "hidden" }}
            />
            <div className="mt-4 w-fit mx-auto">
              <button
                className="text-white bg-purple-900 w-28 rounded-full p-2"
                onClick={onCrop}
              >
                Submit
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default PictureUploadWidget;
