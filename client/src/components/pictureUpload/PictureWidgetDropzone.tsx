import { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import UploadIcon from "@mui/icons-material/Upload";

interface Props {
  setFiles: (files: any) => void;
}

const dzStyles = {
  border: "dashed 3px #eee",
  borderColor: "#eee",
  borderRadius: "5px",
  paddingTop: "30px",
  textAlign: "center" as const,
  height: 200,
};

const dzActive = {
  borderColor: "green",
};

// Drop zone for uploading picture
const PictureWidgetDropzone = ({ setFiles }: Props) => {
  // When file is dropped in or uploaded, set the file in the picture upload widget with the preview set to an object URL created from the blob
  const onDrop = useCallback(
    (acceptedFiles: object[]) => {
      setFiles(
        acceptedFiles.map((file: any) =>
          Object.assign(file, {
            preview: URL.createObjectURL(file),
          })
        )
      );
    },
    [setFiles]
  );

  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  return (
    // Set styles based on active state of drop zone
    <div
      {...getRootProps()}
      style={isDragActive ? { ...dzStyles, ...dzActive } : dzStyles}
    >
      {/* Drop zone */}
      <input {...getInputProps()} />
      <UploadIcon fontSize="large" className="mt-8" />
      <h2>Drop image here</h2>
    </div>
  );
};

export default PictureWidgetDropzone;
