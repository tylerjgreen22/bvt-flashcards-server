import { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import UploadIcon from "@mui/icons-material/Upload";

interface Props {
  setFiles: (files: any) => void;
}

const PictureWidgetDropzone = ({ setFiles }: Props) => {
  const dzStyles = {
    border: "dashed 3px #eee",
    borderColor: "#eee",
    borderRadius: "5px",
    paddingTop: "30px",
    textAlign: "center" as "center",
    height: 200,
  };

  const dzActive = {
    borderColor: "green",
  };

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
    <div
      {...getRootProps()}
      style={isDragActive ? { ...dzStyles, ...dzActive } : dzStyles}
    >
      <input {...getInputProps()} />
      <UploadIcon fontSize="large" className="mt-8" />
      <h2>Drop image here</h2>
    </div>
  );
};

export default PictureWidgetDropzone;
